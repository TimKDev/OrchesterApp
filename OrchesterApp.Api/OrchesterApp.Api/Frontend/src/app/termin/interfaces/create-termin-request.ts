export interface CreateTerminRequest {
  name: string,
  terminArt: number,
  startZeit: Date,
  endZeit: Date,
  straße: string,
  hausnummer: string,
  postleitzahl: string,
  stadt: string,
  zusatz?: string,
  latitude?: number,
  longitude?: number,
  noten: number[],
  uniform: number[],
  orchestermitgliedIds: string[],
  frist?: string,
  ersteWarnungVorFrist?: string
}