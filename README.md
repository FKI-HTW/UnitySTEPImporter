# UnitySTEPImporter

## Development

The Unity project that holds the package and the project configuration is on the `develop` branch. </br>
Once a new Version is ready to be published create a pull request from the `develop` to the `main` branch </br> 
The version of the package that can be used to import it into Unity is on the `upm` branch. </br>

## Folder Structure

> `Assets` : all assets used for the development of the package are placed here </br>
>> assets should adhere to the normal unity folder structure (`Scene`, `Resources`, `Materials` etc.) </br>
>> `/UnitySTEPImporter` : all assets that are part of the final package are placed here </br>
>>> `/Editor/Scripts` : all editor scripts such as custom editors or settings are placed here </br>
>>> `/Runtime/Scripts` : all runtime scripts are placed here </br>
>>> `/Samples~` : optional samples of the package are placed here </br>
>>> `/Resources` : additional resources like example meshes are placed here </br>

`Samples` should have the following folder structure:
> `Samples~`
>> `/"SampleName"`
>>> normal Unity folder structure

## Package Installation and user guide

To import this package into Unity the import via URL can be used.</br>
For example: `https://github.com/CENTIS-HTW/UnitySTEPImporter.git#<version>` </br>

For further details, please refer to [this readme](Assets/UnitySTEPImporter/README.md) inside the package.

### Pushing the Unity package to the upm branch

This repository uses a subtree for the Unity Package.
That way the valid Unity Package and the development Unity project that contains it can be placed into one repository.

The development process stays the same, while the release of a new version of the package takes place on the `upm` branch.
You can find further details [here](https://www.patreon.com/posts/25070968)

**IMPORTANT**</br>
Check that the `Samples` folder located under `Assets/` is renamed to `Samples~` before creating a new release!
Otherwise Unity will throw an error stating that the contained scripts can't be compiled.
This practice follows the Unity guidelines for creating the package structure.
This only has to be done for the versions on the upm branch!

Before you start, check that the version of the package under `Assets/UnitySTEPImporter/package.json` is correct.
Also check that `Assets/UnitySTEPImporter/CHANGELOG.md` is updated, to reflect the changes made.

Note that `"version"` needs to be replaced by the version number that you want to release.
```
git subtree split --prefix=Assets/UnitySTEPImporter --branch upm
git tag "version" upm
git push origin upm --tags
```

To delete a wrong tag:
```
git tag -d tagName
```
If the wrong tag is already pushed:
```
git push origin :tagName
```