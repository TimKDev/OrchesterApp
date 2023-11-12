import { Adresse } from "./adresse";

export interface UpdateSpecificMitgliederRequest{
  id: string, 
  adresse: Adresse, 
  geburtstag: Date, 
  telefonnummer: string, 
  handynummer: string, 
}