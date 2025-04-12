export interface UpdateSpecificMitgliederRequest{
  id: string, 
  straße: string, 
  hausnummer: string,
  postleitzahl: string,
  stadt: string, 
  zusatz: string
  geburtstag: Date, 
  telefonnummer: string, 
  handynummer: string, 
  image?: string,
}