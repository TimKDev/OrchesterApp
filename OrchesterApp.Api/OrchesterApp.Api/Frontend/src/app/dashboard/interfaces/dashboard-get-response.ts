import { DropdownItem } from "src/app/core/interfaces/dropdown-item"

export interface DashboardGetResponse {
  nextTermins: TerminOverview[],
  rückmeldungsDropdownItems: DropdownItem[],
  terminArtDropdownItems: DropdownItem[],
  birthdayList: BirthdayListEntry[]
}

export interface BirthdayListEntry {
  name: string,
  image?: string,
  birthday: Date
}

export interface TerminOverview {
  terminId: string,
   name: string,
   terminArt?: number,
   startZeit: Date,
   endZeit: Date,
   zugesagt: number,
   image?: string,
   fristAsDate?: Date,
   ersteWarnungVorFristAsDate?: Date
}