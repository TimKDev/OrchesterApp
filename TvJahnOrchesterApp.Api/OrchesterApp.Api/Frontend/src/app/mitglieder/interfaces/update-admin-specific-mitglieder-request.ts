import { Adresse } from "./adresse"

export interface UpdateAdminSpecificMitgliederRequest{
  id: string, 
  vorname: string, 
  nachname: string, 
  straße: string, 
  hausnummer: string,
  postleitzahl: string,
  stadt: string, 
  zusatz: string 
  geburtstag: string, 
  telefonnummer: string, 
  handynummer: string, 
  defaultInstrument: number, 
  defaultNotenStimme: number, 
  mitgliedsStatus: number, 
  memberSince: string, 
  positionIds: number[],
  image?: string
}