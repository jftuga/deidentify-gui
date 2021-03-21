# deidentify-gui
An easy to use GUI for the [deidentify](https://github.com/jftuga/deidentify) command-line program

**Note: This is currently a work in progress!**

## To Do List - C#
* status textbox should update sooner than it does
* Implement `Save` button
* Don't hard code program name, version, etc. in `About` window
* Work on `Deidentify` and `Debug` when text is manually entered and added via `Open`

## To Do List - XAML
* Add XAML One-Way bindings in `MainWindow.xaml`
* Place text into resource file and then use: `Content={x:Static properties:Resources.Something}`
* For all labels, add: `Label.Target="{Binding ElementName=TextBox_Something}"`
