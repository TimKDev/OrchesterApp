export interface LoginResponse{
  id: string,
  name: string,
	email: string,
	token: string,
	refreshToken: string,
  userRoles: string[]
}