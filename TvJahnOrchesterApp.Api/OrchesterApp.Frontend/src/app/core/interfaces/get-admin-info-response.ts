export interface GetAdminInfoResponse {
  orchesterMitgliedsId: string,
  userId: string | null,
  email: string | null,
  lastLogin: Date | null,
  orchesterMitgliedsName: string
}