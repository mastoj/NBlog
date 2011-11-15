$WHITE = [System.Drawing.ColorTranslator]::ToOle([System.Drawing.Color]::White)
$BLACK = [System.Drawing.ColorTranslator]::ToOle([System.Drawing.Color]::Black)
$RED = [System.Drawing.ColorTranslator]::ToOle([System.Drawing.Color]::Red)

$properties = $dte.Properties("FontsAndColors", "TextEditor")
$fontsAndColorsItems = $properties.Item("FontsAndColorsItems").Object
$plainTextColors = $fontsAndColorsItems.Item("Plain Text")

$colors = @{}
$colors.White = $WHITE

$colors.Black = $BLACK

$colors.Red = $RED

function Set-FontSize($size) {
    $properties.Item("FontSize").Value = $size
}

function Get-FontSize($size) {
    Write-Host "Current font size: " + $properties.Item("FontSize").Value
}

function Set-FontColor($color) {
	$plainTextColors.Foreground = $colors[$color]
}

function Get-FontColor() {
    Write-Host "Current front color: " + $plainTextColors.Foreground
}

Register-TabExpansion 'Set-FontColor' @{
	'Color' = { $colors.Keys }
}

Export-ModuleMember Set-FontColor
Export-ModuleMember Set-FontSize
