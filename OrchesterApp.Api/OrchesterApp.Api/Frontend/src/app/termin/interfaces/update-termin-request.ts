import { FileItem } from "../components/container/update-termin-modal/update-termin-modal.component"

export interface UpdateTerminRequest {
  terminId?: string,
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
  orchestermitgliedIds: string[] | null,
  weitereInformationen?: string,
  image?: string,
  dokumente: string[],
  shouldEmailBeSend: boolean
}

export interface UpdateTerminModal {
  terminId?: string,
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
  orchestermitgliedIds: string[] | null,
  weitereInformationen?: string,
  image?: string,
  dokumente: FileItem[],
  shouldEmailBeSend: boolean
}