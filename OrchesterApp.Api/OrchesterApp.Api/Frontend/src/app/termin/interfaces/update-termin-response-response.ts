export interface UpdateTerminResponseRequest {
  terminId: string,
  orchesterMitgliedsId: string, 
  zugesagt: number, 
  kommentarZusage?: string, 
  istAnwesend: boolean, 
  kommentarAnwesenheit?: string
}