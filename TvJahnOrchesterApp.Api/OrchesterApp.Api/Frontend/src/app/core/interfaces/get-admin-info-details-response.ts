export interface GetAdminInfoDetailsResponse {
  orchesterMitgliedsId: string,
  userId: string | null,
  registerKeyExpirationDate: Date,
  email: string | null,
  accountLocked: boolean,
  lastLogin: Date | null,
  firstLogin: Date | null,
  roleNames: string[],
  orchesterMitgliedsName: string
}