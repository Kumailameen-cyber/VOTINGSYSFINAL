import { Router } from 'express';
import { AuthController } from '../controllers/authController';
import { validationMiddleware } from '../middleware/validationMiddleware';

const router = Router();
const authController = new AuthController();

router.post('/register', validationMiddleware.validateRegistration, authController.register);
router.post('/login', validationMiddleware.validateLogin, authController.login);

export default router;