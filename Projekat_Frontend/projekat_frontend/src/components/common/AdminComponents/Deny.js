import React, { useContext } from 'react'
import Modal from "../../UI/Modal/Modal";
import classes from '../../auth/Form.module.css'
import axios from 'axios'
import AuthContext from "../../../Contexts/auth-context";


const Deny = (props) => {
    var ctx = useContext(AuthContext);

    const DenyHandler = async() => {
        let reason = document.getElementById('reason')
        console.log(props.username)
        try {
            const response = await axios.post(process.env.REACT_APP_SERVER_URL + 'users/deny', {
                UserName: props.username,
                IsAccepted: false,
                Reason: reason.value,
            }, {
                headers: {
                    Authorization: `Bearer ${ctx.user.Token}`
                }
            });
    
            if (response.data){
                alert(response.data);
                props.onClose()
            }
        }
        catch (error) {
            alert(error);
        }
    }
    

    return (
        <Modal onClose={props.onClose} className={classes.login}>
            <h4>Please state why is {props.username} account denied verification</h4>
            <textarea id='reason'></textarea>
            <center>
                <button onClick={DenyHandler}>Deny</button>
                <button onClick={props.onClose}>Cancle</button>
            </center>
        </Modal>
    )
}

export default Deny