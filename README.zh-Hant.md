# FolderTagFauna [![Static Badge](https://img.shields.io/badge/lang-en-red)](https://github.com/Max46656/FolderTagFauna/blob/main/README.md)

## 簡介

將格式化的字串作為標籤以控制資料夾的工具。  
在這套工具中，資料夾名稱將被分為兩種類型：

1. 棲息地資料夾：這種資料夾在名稱中僅包含標籤，其作為你分類動物資料夾時的結構使用。格式如下「^\\[(.*?)\\]$」。
2. 動物資料夾：你拿來使用的資料夾，比方說去年跟家人旅行的照片、喜歡的樂團專輯等。格式如下「.\*\\[(.*?)\\].\*」。

這套工具以本格式作為標籤「\[Tag1(Tag2)(Tag3)]FolderName」，  
根據你的需求，你可以使用多餘一個標籤組「\[Tag1(Tag2)(Tag3)]FolderName\[Date][Version]」，
這些標籤組均會被視為標籤。

目前本工具組包含以下工具

## [FolderSortingHat](https://github.com/Max46656/FolderTagFauna/blob/main/FolderSortingHat/README.zh-Hant.md)

魔法分類帽，將資料夾帶領到他們的歸屬。
