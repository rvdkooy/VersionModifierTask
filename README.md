VersionModifierTask
===================

Version Modifier Task is a MSBuild Task that lets you easiliy change your existing assemblyinfo file with a predefined pattern.

There are several examples on the internet that allows you to create your assemblyinfo file during a build or use a regular expression to change it. 
This can be done much more easier by using version modifier task


Example:
you want to change your version during a build from ```1.2.0.0``` to ```1.2.[currentyear].[revision]```

With version modifier task you can accomplish this with a pattern defined in your task:
Tell the version modifier task where your assemblyinfo file is located
transform the version with the following pattern: ```[-].[-].[date=yyyy].[$revision]```
'''
Very easy, and most important: you can maintain all your statics in your existing assemblyinfo file, 
your build server will change only the the things that should be changed.

Setting up your VersionModifierTask


1. Download the lastest VersionModifierTask DLL

2. Include the following code snippet in your MSBuild Script:
```
<UsingTask TaskName="ModifyVersion" AssemblyFile="{anylocation}\MSBuild.VersionModifierTask.dll" />
```       
3. Getting the your revision number from source control and put this in a variable ```$(LastChangedRevision)```
	
4. Change the AssemblyInfo file with a year and the LastChangedRevision
```    
<Target Name="AnyName">
        <ModifyVersion AssemblyInfoPath="{anylocation}\properties\assemblyinfo.cs"
        AssemblyVersionPattern="[-].[-].[2011].[$(LastChangedRevision)]"  />
</Target>
```
notes:
the following properties can be set on the task:

<table>
  <tr>
    <td>AssemblyInfoPath</td>
    <td>The location of the AssemblyInfo.cs file you want to modify, this can be an absolute or relative 
    path depending on your needs (you should include the file name)</td>
  </tr>
  <tr>
    <td>AssemblyVersionPattern *</td>
    <td>The pattern used for modifying the AssemblyVersion located in the AssemblyInfo file</td>
  </tr>
  <tr>
    <td>AssemblyFileVersionPattern *</td>
    <td>The pattern used for modifying the AssemblyFileVersion located in the AssemblyInfo file</td>
  </tr>
  tr>
    <td>AssemblyInformationalVersionPattern *</td>
    <td>The pattern used for modifying the AssemblyInformationalVersion located in the AssemblyInfo file</td>
  </tr>
</table>


* The following patterns can be used (the must be enclosed in brackets):

<table>
  <tr>
    <td>-</td>
    <td>the position will be ignored</td>
  </tr>
  <tr>
    <td>date=</td>
    <td>the position will be replaced with a date format (ddMMyyyy)</td>
  </tr>
  <tr>
    <td>0..9</td>
    <td>the position will be replaced with the given number</td>
  </tr>
  <tr>
    <td>$(anyvariable)</td>
    <td>the position will be replaced with the given variable (in here you could pass the revision number retrieved from your favorite version control system)</td>
  </tr>
</table>

<dl>
  <dt>Caution:</dt>
  <dd>The number of positions used in AssemblyVersion or AssemblyFileVersion should match the number of positions in the pattern.</dd>
</dl>
* 1.0.0.0 --> with pattern [-].2.4.4 --> will result in 1.2.4.4
* 1.0.0 --> with pattern [-].2.4.4 --> will fail
* 1.0 --> with pattern 10.2 --> will result in 10.2


Finally, run your build script

Good luck!
