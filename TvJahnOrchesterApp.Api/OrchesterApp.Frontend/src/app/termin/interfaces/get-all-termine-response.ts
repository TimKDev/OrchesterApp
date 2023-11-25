export interface GetAllTermineResponse{
  terminId: string, 
  name: string, 
  terminArt?: number, 
  terminStatus?: number, 
  startZeit: Date, 
  endZeit: Date, 
  zugesagt: number, 
  istAnwesend: boolean, 
  noResponse: number, 
  positiveResponse: number, 
  negativeResponse: number
}
