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
  memberSinceInYears?: number
}

export interface Adresse {
  straße: string, 
  hausnummer: string,
  postleitzahl: string,
  stadt: string, 
  zusatz: string
}