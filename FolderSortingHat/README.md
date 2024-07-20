# FolderSortingHat [![Static Badge](https://img.shields.io/badge/lang-zh--tw-green)](https://github.com/Max46656/FolderTagFauna/blob/main/FolderSortingHat/README.zh-Hant.md)


## Introduction

As usage changes over time, a folder may need to be categorized into different locations.
For this need, you can use the tags of the animal folder to guide it to its habitat.

For example, you can add animal folders in an SD card and classify photos into them, and then archive them into habitat folders based on tags.
Or, after initially classifying memes in the browser download folder, directly archive them to the habitat.

## Installation

Choose the latest release of this project and download the FolderSortingHat.
This project only supports Windows systems.

## Usage

1. Specify the parent folder of the habitat in "Habitat Folder Path". The parent folder can have non-habitat folders, the program only processes folders with tags.
2. (Optional) Use "Create Habitat Map" to scan the parent folder and create a mapping of tags to addresses.
   The file name is "{ParentFolder}.csv".
You can also specify the location and name of the habitat map.
3. If this is your first time scanning the habitat folders,
the program will create an exclusion file "excludeTag.csv" in the parent folder.
You can edit this file to exclude specific tags from recognition in habitat and animal folders during the scan.
4. Specify the parent folder of the animals in "Animal Folder Path".
The parent folder can have non-animal folders, the program only processes folders with tags.
5. If the existing habitat folders have been manually changed and the map is no longer reliable,
select "Recreate Habitat Map".
6. Press "Start Migration".

## Debugging

FolderSortingHat displays the program's operation status and error information in the window and logs the operation process.
You can view more detailed information in "FolderSortingHat.log".

## Technologies Used

- WPF Application: .Net framework 4.7.2, .Net 8.0
- Libraries: CsvHelper
