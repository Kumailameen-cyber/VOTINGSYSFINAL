import { Router } from 'express';
import { VoteController } from '../controllers/voteController';
import { authMiddleware } from '../middleware/authMiddleware';
import { roleMiddleware } from '../middleware/roleMiddleware';

const router = Router();
const voteController = new VoteController();

// Route for casting a vote
router.post('/cast', authMiddleware, roleMiddleware(['voter']), voteController.castVote);

// Route for retrieving vote results
router.get('/results', authMiddleware, roleMiddleware(['candidate']), voteController.getResults);

export default router;