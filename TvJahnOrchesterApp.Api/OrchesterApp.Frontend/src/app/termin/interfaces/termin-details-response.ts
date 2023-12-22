import { DropdownItem } from "src/app/core/interfaces/dropdown-item";

export interface TerminDetailsResponse {
  termin: TerminDetails,
  terminRückmeldung: TerminRückmeldung,
  terminArtenDropdownValues: DropdownItem[],
  terminStatusDropdownValues: DropdownItem[],
  responseDropdownValues: DropdownItem[],
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
  longitude?: number
}

export interface TerminRückmeldung {
  zugesagt: number, 
  kommentarZusage?: string, 
  rückmeldungDurchAnderesOrchestermitglied?: string, 
  istAnwesend: boolean, 
  kommentarAnwesenheit?: string
}