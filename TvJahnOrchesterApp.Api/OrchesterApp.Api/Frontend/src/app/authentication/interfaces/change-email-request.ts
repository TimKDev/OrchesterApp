export interface ChangeEmailRequest{
  oldEmail: string,
  password: string, 
  newEmail: string,
  clientUri: string
}