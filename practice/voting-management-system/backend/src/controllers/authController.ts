export class AuthController {
    constructor(private authService: AuthService) {}

    async register(req: Request, res: Response) {
        try {
            const user = await this.authService.register(req.body);
            res.status(201).json({ message: 'User registered successfully', user });
        } catch (error) {
            res.status(400).json({ message: error.message });
        }
    }

    async login(req: Request, res: Response) {
        try {
            const { token, user } = await this.authService.login(req.body);
            res.status(200).json({ message: 'Login successful', token, user });
        } catch (error) {
            res.status(401).json({ message: error.message });
        }
    }
}