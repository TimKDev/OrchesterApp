import { DropdownItem } from "src/app/core/interfaces/dropdown-item";
import { Adresse } from "./adresse";

export interface GetSpecificMitgliederResponse {
  orchesterMitglied: {
    id: string,
    vorname: string,
    nachname: string,
    adresse: Adresse,
    geburtstag: Date,
    telefonnummer: string,
    handynummer: string,
    defaultInstrument?: number,
    defaultNotenStimme?: number,
    memberSince?: Date,
    memberSinceInYears?: number,
    orchesterMitgliedsStatus?: number,
    positions: number[],
    image?: string,
  },
  notenstimmeDropdownItems: DropdownItem[],
  instrumentDropdownItems: DropdownItem[],
  mitgliedsStatusDropdownItems: DropdownItem[],
  positionDropdownItems: DropdownItem[],
}

