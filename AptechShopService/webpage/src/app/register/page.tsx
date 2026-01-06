'use client';
import { useState } from 'react';
import { Form, Button, Container, Alert } from 'react-bootstrap';
import { useRouter } from 'next/navigation';
import { authService } from '@/services/auth.service';

export default function RegisterPage() {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    confirmPassword: ''
  });
  const [error, setError] = useState('');
  const router = useRouter();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (formData.password !== formData.confirmPassword) {
      setError('Passwords do not match');
      return;
    }

    try {
      await authService.register({ 
        username: formData.username, 
        email: formData.email, 
        password: formData.password 
      });
      router.push('/login');
    } catch (err: any) {
      setError('Registration failed. Try again.');
      console.error(err);
    }
  };

  return (
    <Container className="mt-5" style={{ maxWidth: '400px' }}>
      <h2 className="mb-4 text-center">Register</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <Form onSubmit={handleSubmit}>
        <Form.Group className="mb-3">
          <Form.Label>Username</Form.Label>
          <Form.Control 
            type="text" 
            name="username"
            placeholder="Enter username" 
            value={formData.username}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label>Email address</Form.Label>
          <Form.Control 
            type="email" 
            name="email"
            placeholder="Enter email" 
            value={formData.email}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label>Password</Form.Label>
          <Form.Control 
            type="password" 
            name="password"
            placeholder="Password" 
            value={formData.password}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label>Confirm Password</Form.Label>
          <Form.Control 
            type="password" 
            name="confirmPassword"
            placeholder="Confirm Password" 
            value={formData.confirmPassword}
            onChange={handleChange}
            required
          />
        </Form.Group>

        <Button variant="primary" type="submit" className="w-100">
          Register
        </Button>
      </Form>
    </Container>
  );
}
