<#
.SYNOPSIS
    SuperSpace 终极打包大师 - 微软官方标准版
    集成：dotnet publish, MakeAppx, SignTool, MSIXBundle
#>

Add-Type -AssemblyName System.Windows.Forms
Add-Type -AssemblyName System.Drawing

# --- [1. 自动化环境配置] ---
$ConfigFile = Join-Path $PSScriptRoot ".pack_config.json"
function Get-DefaultConfig {
    return @{ 
        ProjName="SuperSpace"; 
        PubID="CN=YourPublisherID"; 
        SDKPath=""; 
        SourcePath=$PSScriptRoot; 
        OutputPath=(Join-Path $PSScriptRoot "Deploy_Output");
        CertPath="";
        CertPwd=""
    }
}
$Script:Cfg = if (Test-Path $ConfigFile) { ConvertFrom-Json (Get-Content $ConfigFile -Raw) } else { Get-DefaultConfig }

function Get-WinSDKTools {
    $base = "${env:ProgramFiles(x86)}\Windows Kits\10\bin"
    if (Test-Path $base) {
        $latest = Get-ChildItem $base | Where-Object { $_.Name -match "^10\." } | Sort-Object Name -Descending | Select-Object -First 1
        if ($latest) {
            return @{
                MakeAppx = Join-Path $latest.FullName "x64\makeappx.exe"
                SignTool = Join-Path $latest.FullName "x64\signtool.exe"
            }
        }
    }
    return $null
}

# --- [2. UI 界面构建] ---
$form = New-Object System.Windows.Forms.Form
$form.Text = "SuperSpace 终极打包大师 (Microsoft Standard)"; $form.Size = "700,980"; $form.StartPosition = "CenterScreen"
$form.Font = New-Object System.Drawing.Font("Segoe UI", 9)
$form.BackColor = [System.Drawing.Color]::White

function New-Label($txt, $top) {
    $l = New-Object System.Windows.Forms.Label; $l.Text = $txt; $l.Location = "30,$top"; $l.AutoSize = $true; $form.Controls.Add($l)
}

# 路径设置区
New-Label "项目根路径:" 20
$txtSrc = New-Object System.Windows.Forms.TextBox; $txtSrc.Text = $Cfg.SourcePath; $txtSrc.Location = "160,17"; $txtSrc.Width = 400; $form.Controls.Add($txtSrc)

New-Label "输出路径:" 55
$txtOut = New-Object System.Windows.Forms.TextBox; $txtOut.Text = $Cfg.OutputPath; $txtOut.Location = "160,52"; $txtOut.Width = 400; $form.Controls.Add($txtOut)

New-Label "PFX 证书路径:" 90
$txtCert = New-Object System.Windows.Forms.TextBox; $txtCert.Text = $Cfg.CertPath; $txtCert.Location = "160,87"; $txtCert.Width = 400; $form.Controls.Add($txtCert)

New-Label "证书密码:" 125
$txtPwd = New-Object System.Windows.Forms.TextBox; $txtPwd.PasswordChar = "*"; $txtPwd.Text = $Cfg.CertPwd; $txtPwd.Location = "160,122"; $txtPwd.Width = 200; $form.Controls.Add($txtPwd)

# 选项区
$grp = New-Object System.Windows.Forms.GroupBox; $grp.Text = "打包策略 (Microsoft Standards)"; $grp.Location = "30,170"; $grp.Size = "620,130"; $form.Controls.Add($grp)
$chkZip = New-Object System.Windows.Forms.CheckBox; $chkZip.Text = "Zip Portable"; $chkZip.Location = "20,30"; $chkZip.Checked = $true; $grp.Controls.Add($chkZip)
$chkSingle = New-Object System.Windows.Forms.CheckBox; $chkSingle.Text = "Single MSIX (x64/ARM64)"; $chkSingle.Location = "150,30"; $chkSingle.Width = 180; $chkSingle.Checked = $true; $grp.Controls.Add($chkSingle)
$chkCombined = New-Object System.Windows.Forms.CheckBox; $chkCombined.Text = "Combined MSIX (Fat Package)"; $chkCombined.Location = "340,30"; $chkCombined.Width = 220; $chkCombined.Checked = $true; $grp.Controls.Add($chkCombined)
$chkBundle = New-Object System.Windows.Forms.CheckBox; $chkBundle.Text = "MSIXBundle (Official Recommended)"; $chkBundle.Location = "20,65"; $chkBundle.Width = 300; $chkBundle.Checked = $true; $grp.Controls.Add($chkBundle)
$chkAOT = New-Object System.Windows.Forms.CheckBox; $chkAOT.Text = "Native AOT"; $chkAOT.Location = "20,95"; $grp.Controls.Add($chkAOT)
$chkTrim = New-Object System.Windows.Forms.CheckBox; $chkTrim.Text = "Code Trimming"; $chkTrim.Location = "150,95"; $chkTrim.Checked = $true; $grp.Controls.Add($chkTrim)

# 日志区
$txtLog = New-Object System.Windows.Forms.TextBox; $txtLog.Multiline = $true; $txtLog.Location = "30,320"; $txtLog.Size = "620,450"; $txtLog.ReadOnly = $true; $txtLog.BackColor = "Black"; $txtLog.ForeColor = "Lime"; $txtLog.Font = New-Object System.Drawing.Font("Consolas", 9); $txtLog.ScrollBars = "Vertical"; $form.Controls.Add($txtLog)

$btnGo = New-Object System.Windows.Forms.Button; $btnGo.Text = "开始标准打包流程"; $btnGo.Location = "30,790"; $btnGo.Size = "620,60"; $btnGo.BackColor = "LightBlue"; $btnGo.Font = New-Object System.Drawing.Font("Segoe UI", 12, "Bold"); $form.Controls.Add($btnGo)

$lblStatus = New-Object System.Windows.Forms.Label; $lblStatus.Text = "就绪"; $lblStatus.Location = "30,860"; $lblStatus.AutoSize = $true; $form.Controls.Add($lblStatus)

# --- [3. 核心打包逻辑] ---
function Update-Log($msg) {
    $ts = Get-Date -Format "HH:mm:ss"
    $txtLog.AppendText("[$ts] $msg`r`n")
    $txtLog.SelectionStart = $txtLog.TextLength
    $txtLog.ScrollToCaret()
    $lblStatus.Text = $msg
    [System.Windows.Forms.Application]::DoEvents()
}

$btnGo.Add_Click({
    $globalSw = [System.Diagnostics.Stopwatch]::StartNew()
    $txtLog.Clear()
    $tools = Get-WinSDKTools
    if (!$tools) { [MessageBox]::Show("无法定位 Windows SDK 工具 (MakeAppx/SignTool)"); return }

    try {
        # 初始化与配置导出
        $out = $txtOut.Text
        if (Test-Path $out) { Remove-Item $out -Recurse -Force -ErrorAction SilentlyContinue }
        New-Item -ItemType Directory $out -Force | Out-Null
        
        $archs = @("x64", "arm64")
        $msixFiles = @()

        foreach ($arch in $archs) {
            $stepSw = [System.Diagnostics.Stopwatch]::StartNew()
            Update-Log "--- 正在启动架构: $arch ---"
            
            # A. Dotnet Publish (遵循 .NET 部署最佳实践)
            $pubDir = Join-Path $out "Pub_$arch"
            $publishArgs = @("publish", "`"$($txtSrc.Text)`"", "-c", "Release", "-r", "win-$arch", "--self-contained", "true", "-o", "`"$pubDir`"", "/p:PublishReadyToRun=true")
            if ($chkTrim.Checked) { $publishArgs += "/p:PublishTrimmed=true" }
            if ($chkAOT.Checked) { $publishArgs += "/p:PublishAOT=true" }
            
            Update-Log "执行命令: dotnet $($publishArgs -join ' ')"
            $proc = Start-Process dotnet -ArgumentList $publishArgs -NoNewWindow -PassThru -Wait
            if ($proc.ExitCode -ne 0) { throw "$arch 发布失败" }
            Update-Log "编译完成。耗时: $($stepSw.Elapsed.TotalSeconds.ToString('F2'))s"

            # B. 准备 MSIX 封装布局
            $stage = Join-Path $out "Stage_$arch"
            New-Item $stage -ItemType Directory | Out-Null
            Copy-Item "$pubDir\*" -Destination $stage -Recurse
            Copy-Item (Join-Path $txtSrc.Text "Package.appxmanifest") -Destination (Join-Path $stage "AppxManifest.xml")
            if (Test-Path (Join-Path $txtSrc.Text "Assets")) { Copy-Item (Join-Path $txtSrc.Text "Assets") -Destination $stage -Recurse }

            # C. 封装与签名
            $msixPath = Join-Path $out "$arch`_Package.msix"
            Update-Log "正在使用 MakeAppx 生成 MSIX..."
            & $tools.MakeAppx pack /d $stage /p $msixPath /o /nv | Out-Null

            if (Test-Path $txtCert.Text) {
                Update-Log "正在进行数字签名..."
                & $tools.SignTool sign /fd SHA256 /f $txtCert.Text /p $txtPwd.Text $msixPath | Out-Null
            }
            
            $msixFiles += $msixPath
            if ($chkZip.Checked) { Compress-Archive -Path "$pubDir\*" -DestinationPath (Join-Path $out "Portable_$arch.zip") }
        }

        # D. 生成多架构合并版 (Fat MSIX)
        if ($chkCombined.Checked) {
            $stepSw = [System.Diagnostics.Stopwatch]::StartNew()
            Update-Log "正在生成多架构合并 MSIX (Combined)..."
            $multiStage = Join-Path $out "Stage_Combined"
            foreach($a in $archs) {
                $aDir = New-Item (Join-Path $multiStage $a) -ItemType Directory
                Copy-Item (Join-Path $out "Stage_$a\*") -Destination $aDir.FullName -Recurse
            }
            Copy-Item (Join-Path $txtSrc.Text "Package.appxmanifest") -Destination "$multiStage\AppxManifest.xml"
            $combinedMsix = Join-Path $out "Full_Universal.msix"
            & $tools.MakeAppx pack /d $multiStage /p $combinedMsix /o /nv | Out-Null
            if (Test-Path $txtCert.Text) { & $tools.SignTool sign /fd SHA256 /f $txtCert.Text /p $txtPwd.Text $combinedMsix | Out-Null }
            Update-Log "合并版完成。耗时: $($stepSw.Elapsed.TotalSeconds.ToString('F2'))s"
        }

        # E. 生成官方 MSIXBundle
        if ($chkBundle.Checked) {
            $stepSw = [System.Diagnostics.Stopwatch]::StartNew()
            Update-Log "正在构建标准 MSIXBundle..."
            $mapFile = Join-Path $out "bundlemap.txt"
            "[Files]" | Out-File $mapFile
            foreach($f in $msixFiles) { "`"$f`" `"$(Split-Path $f -Leaf)`"" | Out-File $mapFile -Append }
            $bundlePath = Join-Path $out "Final_App.msixbundle"
            & $tools.MakeAppx bundle /f $mapFile /p $bundlePath /o | Out-Null
            if (Test-Path $txtCert.Text) { & $tools.SignTool sign /fd SHA256 /f $txtCert.Text /p $txtPwd.Text $bundlePath | Out-Null }
            Update-Log "Bundle 完成。耗时: $($stepSw.Elapsed.TotalSeconds.ToString('F2'))s"
        }

        # 清理
        Get-ChildItem $out -Directory -Filter "Stage_*" | Remove-Item -Recurse -Force
        Get-ChildItem $out -Directory -Filter "Pub_*" | Remove-Item -Recurse -Force
        
        $globalSw.Stop()
        $msg = "打包成功！总耗时: $($globalSw.Elapsed.TotalSeconds.ToString('F2'))s"
        Update-Log $msg
        [MessageBox]::Show($msg)

    } catch {
        Update-Log "!!! 错误: $($_.Exception.Message)"
    }
})

$form.ShowDialog()