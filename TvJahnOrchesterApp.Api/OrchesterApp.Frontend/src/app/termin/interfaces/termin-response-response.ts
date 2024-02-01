import { DropdownItem } from "src/app/core/interfaces/dropdown-item"

export interface TerminResponseResponse{
  terminId: string, 
  terminName: string, 
  terminRückmeldungsTableEntries: TerminRückmeldungsTableEntry[],
  responseDropdownValues: DropdownItem[]
}

export interface TerminRückmeldungsTableEntry {
  rückmeldungsId: string, 
  vorname: string, 
  nachname: string, 
  vornameOther?: string, 
  nachnameOther?: string, 
  zugesagt: number, 
  kommentarZusage?: string, 
  letzteRückmeldung?: Date, 
  istAnwesend: boolean, 
  kommentarAnwesenheit?: string
}