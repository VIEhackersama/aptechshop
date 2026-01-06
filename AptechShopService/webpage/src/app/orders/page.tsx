'use client';
import { useEffect, useState } from 'react';
import { Container, Table, Spinner, Alert, Badge } from 'react-bootstrap';
import { orderService, Order } from '@/services/order.service';

export default function OrdersPage() {
  const [orders, setOrders] = useState<Order[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    fetchOrders();
  }, []);

  const fetchOrders = async () => {
    try {
      const response = await orderService.getAll();
      setOrders(response.data);
    } catch (err) {
      setError('Failed to load orders.');
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
      <h2 className="mb-4">My Orders</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <Table striped bordered hover responsive>
        <thead>
          <tr>
            <th>Order ID</th>
            <th>Date</th>
            <th>Total Amount</th>
            <th>Status</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id}>
              <td>#{order.id}</td>
              <td>{new Date(order.date).toLocaleDateString()}</td>
              <td></td>
              <td>
                <Badge bg={order.status === 'Completed' ? 'success' : 'warning'}>
                  {order.status}
                </Badge>
              </td>
              <td>
                <button className="btn btn-sm btn-info text-white">View Details</button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
      {!loading && orders.length === 0 && <p className="text-center">No orders found.</p>}
    </Container>
  );
}
