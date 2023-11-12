import { Adresse } from "./adresse";

export interface GetSpecificMitgliederResponse{
  id: string, 
  vorname: string, 
  nachname: string, 
  adresse: Adresse, 
  geburtstag: Date, 
  telefonnummer: string, 
  handynummer: string, 
  defaultInstrument?: number, 
  defaultNotenStimme?: number,  
  memberSince?: Date, 
  memberSinceInYears?: number,
  orchesterMitgliedsStatus?: number,
  positions: number[]
}

