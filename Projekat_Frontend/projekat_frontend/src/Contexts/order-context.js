import React,{useState, useContext} from 'react'
import axios from 'axios';
import AuthContext from './auth-context'

const OrderContext = React.createContext({
    onFetch: ()=>{},
    onFetchNew :()=>{},
    onFetchHistory:()=>{}, 
    onFetchBuyers: ()=>{},
});

export const OrderContextProvider = (props) => {
    const [allOrders,setAllOrders] = useState([]);
    const [buyersOrders,setBuyersOrders] = useState([]);
    const [orderHistory,setOrderHistory] = useState([]);
    const [newOrders,setNewOrders] = useState([]);
    const ctx = useContext(AuthContext)

    //Admin fetches all orders
    const fetchAllHandler=()=>{
        axios.get(process.env.REACT_APP_SERVER_URL+'orders/allOrders',{
            headers: {
              Authorization: `Bearer ${ctx.user.Token}`
            }
          })
        .then(response => {
        if(response.data != null){
            setAllOrders(response.data);
        }
        else
        setAllOrders([])
        });
    }

    //Seller fetches new orders that contain his items and are not shipped 
    const fetchNewHandler=()=>{
        axios.get(process.env.REACT_APP_SERVER_URL+'orders/newOrders?id='+ctx.user.Id, {
            headers: {
              Authorization: `Bearer ${ctx.user.Token}`
            }
          })
        .then(response => {
        if(response.data != null){
          setNewOrders(response.data);
        }
        else
        setNewOrders([])
        });
    }

    //Seller fetches orders that contains his items and are shipped
    const fetchHistoryHandler=()=>{
        axios.get(process.env.REACT_APP_SERVER_URL+'orders/orderHistory?id='+ctx.user.Id,{
            headers: {
              Authorization: `Bearer ${ctx.user.Token}`
            }
          })
        .then(response => {
        if(response.data != null){
            setOrderHistory(response.data);
        }
        else
        setOrderHistory([])
        });
    }

    //Buyer fetches all his orders except canceled
    const fetchBuyersHandler=()=>{
        axios.get(process.env.REACT_APP_SERVER_URL+'orders/myOrders?id='+ctx.user.Id,{
            headers: {
              Authorization: `Bearer ${ctx.user.Token}`
            }
          })
        .then(response => {
            if(response.data != null){
                setBuyersOrders(response.data);
            }
            else
            setBuyersOrders([])
        });
    }

    return (
        <OrderContext.Provider
        value={{
            allOrders:allOrders,
            buyersOrders:buyersOrders,
            orderHistory:orderHistory,
            newOrders:newOrders,
            onFetchAll: fetchAllHandler, 
            onFetchNew: fetchNewHandler, 
            onFetchHistory: fetchHistoryHandler, 
            onFetchBuyers: fetchBuyersHandler, 
            }}>
            {props.children}
        </OrderContext.Provider>
    )

}

export default OrderContext;