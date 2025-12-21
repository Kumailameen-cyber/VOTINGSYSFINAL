import { Router } from 'express';
import { CandidateController } from '../controllers/candidateController';
import { authMiddleware } from '../middleware/authMiddleware';
import { roleMiddleware } from '../middleware/roleMiddleware';

const router = Router();
const candidateController = new CandidateController();

// Route to get all candidates
router.get('/', candidateController.getAllCandidates);

// Route to get a specific candidate by ID
router.get('/:id', candidateController.getCandidateById);

// Route to create a new candidate (protected route)
router.post('/', authMiddleware, roleMiddleware('candidate'), candidateController.createCandidate);

// Route to update a candidate's information (protected route)
router.put('/:id', authMiddleware, roleMiddleware('candidate'), candidateController.updateCandidate);

// Route to delete a candidate (protected route)
router.delete('/:id', authMiddleware, roleMiddleware('candidate'), candidateController.deleteCandidate);

export default router;