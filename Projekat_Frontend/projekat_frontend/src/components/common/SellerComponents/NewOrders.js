import React,{useEffect, Fragment,useContext} from 'react'
import { v4 as uuidv4 } from 'uuid';

import classes from './NewOrders.module.css'
import Card from '../../UI/Card/Card'
import OrderCard from '../OrderCard';

import OrderContext from '../../../Contexts/order-context'
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import AuthContext from '../../../Contexts/auth-context';

const NewOrders = () => {
  const ctx = useContext(OrderContext);
  const authCtx = useContext(AuthContext)
  const navigate = useNavigate(); 
  useEffect(() => {
    ctx.onFetchNew();
  }, []);

  return (
    <Fragment>
    <section className={classes.summary}>
      <h2>New orders</h2>

      <section className={classes.users}>
        {ctx.newOrders.length > 0 ? (
          <Card>
            <ul>
              {ctx.newOrders.map((order) => (
                <>
                <OrderCard
                  key={uuidv4()}
                  id={order.id}
                  Items={order.orderItems}
                  shipped={order.shipped}
                  canceled={order.canceled}
                  minutes={order.minutes}
                />
                </> 
              ))}
            </ul>
          </Card>
        ) : (
          <h2>There's no order made with your articles</h2>
        )}
      </section>
    </section>
  </Fragment>
  )
}

export default NewOrders