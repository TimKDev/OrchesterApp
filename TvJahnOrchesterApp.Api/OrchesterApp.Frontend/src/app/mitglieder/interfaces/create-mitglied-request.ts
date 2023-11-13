export interface CreateMitgliedRequest{
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
  memberSince: string,
  registerKey: string,
  positionIds: number[]
}