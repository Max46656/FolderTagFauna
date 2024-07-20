# FolderTagFauna [![Static Badge](https://img.shields.io/badge/lang-en-red)](https://github.com/Max46656/FolderTagFauna/blob/main/README.zh-Hant.md)

## Introduction

A tool that uses formatted strings as tags to manage folders.  
In this tool, folder names are divided into two types:

1. Habitat Folders: These folders only contain tags in their names and are used to organize your animal folders.  
The format is as follows: "^\\[(.*?)\\]$."
2. Animal Folders: These are the folders you use, such as photos from last year's trip with your family,  
favorite band albums, etc. The format is as follows: ".\*\\[(.*?)\\].\*"

This tool uses the following format for tags: \[Tag1(Tag2)(Tag3)]FolderName.  
Depending on your needs, you can use more than one tag group: \[Tag1(Tag2)(Tag3)]FolderName\[Date][Version].  
All these tag groups will be considered as tags.

Currently, this toolkit includes the following tools:

## [FolderSortingHat](https://github.com/Max46656/FolderTagFauna/blob/main/FolderSortingHat/README.md)

The magic sorting hat that leads folders to their rightful place.
