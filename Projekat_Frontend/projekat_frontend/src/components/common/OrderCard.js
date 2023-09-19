import React,{useContext} from "react";
import axios from 'axios'
import classes from "./OrderCard.module.css";
import AuthContext from '../../Contexts/auth-context'

const OrderCard = (props) => {
  const ctx= useContext(AuthContext)

  const cancleHandler = async()=>{
    try {
      const response = await axios.post(process.env.REACT_APP_SERVER_URL + 'orders/cancleOrder?id='+props.id, {},{
        headers: {
          Authorization: `Bearer ${ctx.user.Token}`
        }
      });

      if (response.data){
        alert(response.data)
        props.onCancle()
      }
      
    }
    catch (error) {
      console.error(error);
    }
  }


  return (
    <li className={classes.user}>
      <div>
        <h4>Order Id: {props.id}</h4>
        {props.Buyer && (<h3>Buyer: {props.Buyer}</h3>)}
        <div className={classes.description}>
          {props.Items.map(element => <p>Item name:<b>{element.item.name}</b> Ordered amount: <b>{element.amount}</b> Price: <b>{element.item.price}$</b>Total: <b>{element.item.price*element.amount}$</b></p>)}
        </div>
        <p>
          Total cost of all items:{" "}
          <b>
            {props.Items.reduce((total, element) => total + element.item.price * element.amount, 0)} +3$ shipping
          </b>
        </p>
        {(!props.shipped && !props.canceled && ctx.user.Role!==3) &&(
          <>
          <h4>Time until arrival {props.minutes}min</h4>
          {ctx.user.Role===1 && (<button onClick={cancleHandler}>Cancel</button>)}
          </>
        )}
        {(props.shipped) && (
          <>
          {ctx.user.Role===1 && (<b>Order has arrived</b>)}
          {(ctx.user.Role===2 || ctx.user.Role===3) && (<b>Order has reached the destination</b>)}

          </>
        )}
        {(props.canceled) && (
          <>
          {(ctx.user.Role===3 || ctx.user.Role===2 ) && (<b>Order has been canceled</b>)}
          </>
        )}
        
       
      </div>
    </li>
  )
}

export default OrderCard