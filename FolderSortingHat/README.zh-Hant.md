# FolderSortingHat

## 簡介

隨著使用階段的改變，一個資料夾需要被歸類入不同的位置。
對於這種需求，可以透過動物資料夾的標籤，使其前往他的棲息地。

例如在SD卡中新增動物資料夾並將照片分類到其中，並根據標籤歸檔到棲息地資料夾。
或是將瀏覽器下載資料夾的迷因在下載資料夾進行初步的歸類後，直接歸檔到棲息地。

## 安裝

選擇本專案最新的Releases，下載其中的FolderSortingHat。
本專案僅支援Windows系統。

## 使用方法

1. 在「棲息地資料夾位置」指定棲息地的母資料夾，母資料夾可以擁有非棲息地的資料夾，程式僅處理僅帶有標籤的資料夾。
2. （非必要）使用「建立棲息地地圖」掃描母資料夾，建立標籤與地址的對應地圖，其檔案名稱為「{母資料夾}.csv」。
   你可以另外指定棲息地地圖的位置與名稱。
3. 若你是第一次掃描棲息地資料夾，程式會在母資料夾中產生一個排除用的「excludeTag.csv」，
   你可以編輯此檔案以在掃描時排除特定標籤於棲息地與動物資料夾的辨識中。
4. 在「動物資料夾位置」指定動物的母資料夾，母資料夾可以擁有非動物的資料夾，程式僅處理帶有標籤的資料夾。
5. 若舊有的棲息地資料夾經過手動更改使地圖已不可信，選取「重新建立棲息地地圖」。
6. 按下「遷徙開始」。

## 除錯

FolderSortingHat會將程式的運作狀態與錯誤資訊呈現於視窗中，也使用log紀錄運作過程。
你可以於「FolderSortingHat.log」中查看更詳細的資訊。

## 使用技術

- WPF 應用程式：.Net framework 4.7.2、.Net 8.0
- 函式庫：CsvHelper