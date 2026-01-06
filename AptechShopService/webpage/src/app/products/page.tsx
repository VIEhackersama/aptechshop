'use client';
import { useEffect, useState } from 'react';
import { Container, Row, Col, Card, Button, Spinner, Alert } from 'react-bootstrap';
import { productService, Product } from '@/services/product.service';

export default function ProductsPage() {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      const response = await productService.getAll();
      setProducts(response.data);
    } catch (err) {
      setError('Failed to load products.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) return (
    <Container className="mt-5 text-center">
      <Spinner animation="border" />
    </Container>
  );

  return (
    <Container className="mt-4">
      <h2 className="mb-4">Products</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <Row>
        {products.map((product) => (
          <Col key={product.id} md={4} className="mb-4">
            <Card>
              {product.imageUrl && <Card.Img variant="top" src={product.imageUrl} style={{ height: '200px', objectFit: 'cover' }} />}
              <Card.Body>
                <Card.Title>{product.name}</Card.Title>
                <Card.Text>
                  {product.description}
                  <br />
                  <strong></strong>
                </Card.Text>
                <Button variant="primary">Add to Cart</Button>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
      {!loading && products.length === 0 && <p className="text-center">No products found.</p>}
    </Container>
  );
}
