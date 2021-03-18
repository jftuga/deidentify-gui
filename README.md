# deidentify-gui
An easy to use GUI for the deidentify command-line program

## To Do List - C#
* status textbox should update sooner than it does
* Deidentified window should **bold** all substitutions
* Implement `Save` button
* Don't hard code program name, version, etc. in `About` window
* Don't hard code `jsonFile` in `Click_Deidentify()`
* Don't hard code `filename`, `outputFile` in `File_Deidentify()`

## To Do List - XAML
* Add XAML One-Way bindings in `MainWindow.xaml`
* Place text into resource file and then use: `Content={x:Static properties:Resources.Something}`
* For all labels, add: `Label.Target="{Binding ElementName=TextBox_Something}"`
* For all textboxes, add: `Text="{Binding Path=Something, Mode=OneWay}"`
