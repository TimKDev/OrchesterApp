import { DropdownItem } from "src/app/core/interfaces/dropdown-item";

export interface TerminDetailsResponse {
  termin: TerminDetails,
  terminRückmeldung: TerminRückmeldung,
  terminArtenDropdownValues: DropdownItem[],
  terminStatusDropdownValues: DropdownItem[],
  responseDropdownValues: DropdownItem[],
  notenDropdownValues: DropdownItem[],
  uniformDropdownValues: DropdownItem[],
}

export interface TerminDetails {
  terminName: string,
  terminArt: number,
  terminStatus: number,
  startZeit: Date,
  endZeit: Date,
  straße?: string,
  hausnummer?: string,
  postleitzahl?: string,
  stadt?: string,
  zusatz?: string,
  latitude?: number,
  longitude?: number,
  noten: number[],
  uniform: number[],
  weitereInformationen?: string,
  image?: string,
  dokumente?: string[]
}

export interface TerminRückmeldung {
  zugesagt: number, 
  kommentarZusage?: string, 
  vornameOther?: string, 
  nachnameOther?: string, 
  istAnwesend: boolean, 
  kommentarAnwesenheit?: string
}