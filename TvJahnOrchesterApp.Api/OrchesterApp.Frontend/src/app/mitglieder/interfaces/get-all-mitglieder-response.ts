export interface GetAllMitgliederResponse{
  id: string, 
  vorname: string, 
  nachname: string, 
  defaultInstrument?: string, 
  memberSinceInYears?: number
}