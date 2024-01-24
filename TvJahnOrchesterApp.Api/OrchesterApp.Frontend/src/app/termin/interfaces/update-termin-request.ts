export interface UpdateTerminRequest {
  terminName: string,
  terminArt: number,
  terminStatus: number,
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
  orchestermitgliedIds: string[]
}