import { DropdownItem } from "src/app/core/interfaces/dropdown-item";

export interface TerminListDataResponse {
  terminData: TermineListData[],
  terminArtenDropdownValues: DropdownItem[],
  terminStatusDropdownValues: DropdownItem[],
  responseDropdownValues: DropdownItem[],
}

export interface TermineListData{
  terminId: string,
  name: string,
  terminArt?: number,
  terminStatus?: number,
  startZeit: Date,
  endZeit: Date,
  zugesagt: number,
  istAnwesend: boolean,
  noResponse: number,
  positiveResponse: number,
  negativeResponse: number,
  image?: string
}
