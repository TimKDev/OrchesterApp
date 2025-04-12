export interface GetAllMitgliederResponse{
  id: string, 
  vorname: string, 
  nachname: string, 
  image?: string,
  defaultInstrument?: string, 
  memberSinceInYears?: number
}