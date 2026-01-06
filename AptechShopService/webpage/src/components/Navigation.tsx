'use client';
import Link from 'next/link';
import { Navbar, Container, Nav, Button } from 'react-bootstrap';
import { useRouter } from 'next/navigation';

export default function Navigation() {
  const router = useRouter();

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    router.push('/login');
  };

  return (
    <Navbar bg="dark" variant="dark" expand="lg" className="mb-4">
      <Container>
        <Navbar.Brand href="/">AptechShop</Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="me-auto">
            <Link href="/" passHref legacyBehavior>
              <Nav.Link>Home</Nav.Link>
            </Link>
            <Link href="/products" passHref legacyBehavior>
              <Nav.Link>Products</Nav.Link>
            </Link>
            <Link href="/orders" passHref legacyBehavior>
              <Nav.Link>Orders</Nav.Link>
            </Link>
          </Nav>
          <Nav>
             <Link href="/login" passHref legacyBehavior>
              <Nav.Link>Login</Nav.Link>
            </Link>
            <Link href="/register" passHref legacyBehavior>
              <Nav.Link>Register</Nav.Link>
            </Link>
             <Button variant="outline-light" size="sm" onClick={handleLogout} className="ms-2">
                Logout
             </Button>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
}
