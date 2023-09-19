import React, { useContext, useState } from "react";

import classes from "./UserCard.module.css";
import axios from "axios";
import AuthContext from '../../../Contexts/auth-context'
import Deny from "./Deny";


const getImageType = (image) => {
  if (image.startsWith('/9j/')) {
    return 'image/jpeg';
  } else if (image.startsWith('iVBORw0KGgo')) {
    return 'image/png';
  } else if (image.startsWith('PHN2Zy')) {
    return 'image/svg+xml';
  } else if (image.startsWith('R0lGODlh')) {
    return 'image/gif';
  } else {
    return '';
  }
};

const UserCard = (props) => {
  const birthDate = new Date(props.BirthDate);
  const day = String(birthDate.getDate()).padStart(2, '0');
  const month = String(birthDate.getMonth() + 1).padStart(2, '0');
  const year = birthDate.getFullYear();
  const formattedDate = `${day}/${month}/${year}`;
  const ctx = useContext(AuthContext);
  const [deny, setDeny] = useState(false);

  const VerifyHandler = async (username) => {
    try {
      const response = await axios.post(process.env.REACT_APP_SERVER_URL + 'users/verify', {
        UserName: username,
        IsAccepted: true,
        Reason: '',
      }, {
        headers: {
          Authorization: `Bearer ${ctx.user.Token}`
        }
      });

      if (response.data)
        props.onVerify();
    }
    catch (error) {
      console.error(error);
    }
  }

  const showDenyHandler = () => {
    setDeny(true);
  };

  const hideDenyFormHandler = () => {
    setDeny(false);
    props.onVerify();
  };

  var imageURL = '';
  if (props.Avatar != null)
    imageURL = `data:${getImageType(props.Avatar)};base64,${props.Avatar}`;


  return (
    <li className={classes.user}>
      <div>
        <h3>Username: {props.id}</h3>
        <div className={classes.description}>
          First Name: {props.FirstName}<br />
          Last Name: {props.LastName}<br />
          Email: {props.Email}<br />
          Address: {props.Address}<br />
          Birth date: {formattedDate}<br />
          Profile picture: <br />
          {props.Avatar && <img className={classes.profilePic} src={imageURL} alt="Profile picture" />}
        </div>
      </div>
      <div className="actions">
        <button onClick={() => VerifyHandler(props.id)} >Verify</button>
        <button onClick={showDenyHandler}>Deny</button>
        {deny && <Deny username={props.id} onClose={hideDenyFormHandler} />}
      </div>
    </li>
  )
}

export default UserCard