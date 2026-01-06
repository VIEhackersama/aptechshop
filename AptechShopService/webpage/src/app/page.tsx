'use client';
import { Button, Container } from 'react-bootstrap';

export default function Home() {
  return (
    <Container className="p-5">
      <h1 className="mb-4">AptechShop Frontend Test</h1>
      <p>This is a temporary frontend to test the backend API.</p>
      <div className="d-flex gap-3">
        <Button variant="primary">Primary Button</Button>
        <Button variant="secondary">Secondary Button</Button>
      </div>
    </Container>
  );
}
