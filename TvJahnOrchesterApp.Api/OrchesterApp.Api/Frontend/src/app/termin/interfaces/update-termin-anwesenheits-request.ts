export interface UpdateTerminAnwesenheitsRequest{
  terminId: string,
  anwesenheitResponseEntries: AnwesenheitResponseEntry[]
}

export interface AnwesenheitResponseEntry {
  orchesterMitgliedsId: string, 
  istAnwesend: boolean, 
  kommentarAnwesenheit?: string 
}