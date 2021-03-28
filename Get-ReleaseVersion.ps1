$json = Get-Content D:\a\deidentify-gui\deidentify-gui\bin\Release\netcoreapp3.1\deidentify-gui.deps.json | ConvertFrom-Json
$prop = $json.targets | Get-Member -MemberType NoteProperty | Select-Object -ExpandProperty Name

foreach( $property in $json.targets.$prop.psobject.properties.name )
{
    if($property.StartsWith("deidentify-gui/")) {
        $slots = $property -split "/"
        $slots[1]
        break
    }
}
