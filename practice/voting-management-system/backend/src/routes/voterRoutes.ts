import { Router } from 'express';
import { VoterController } from '../controllers/voterController';
import { authMiddleware } from '../middleware/authMiddleware';
import { roleMiddleware } from '../middleware/roleMiddleware';

const router = Router();
const voterController = new VoterController();

router.get('/dashboard', authMiddleware, voterController.getDashboard);
router.get('/voting-history', authMiddleware, voterController.getVotingHistory);
router.post('/vote', authMiddleware, roleMiddleware('voter'), voterController.castVote);

export default router;