<%@ Page Language="C#" AutoEventWireup="true" CodeFile="handlereturnapp.aspx.cs" Inherits="handlereturnapp" %>

<!DOCTYPE html>

<html lang="en">



<%--<body>

   <button id="btn" name="btn" value="Back" onclick="back_onclick();">BACK</button>

</body>--%>

     <head>
    <meta http-equiv="Content-Type" content="text/html; charset=ISO-8859-1" />
      <meta name="viewport"
        content="viewport-fit=cover, width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <title>Merchant Response Page</title>

         
       <script type="text/javascript" language="javascript">
           setTimeout(function back_onclick() {
               if (navigator.userAgent.indexOf('Android') > -1) {
                   window.open("http://localhost/#/response-handler", "_self");
               }
               else {
                   window.open("ionic://localhost/#/response-handler", "_self");
               }


           }, 4000);

        </script>
  </head>
  <body data-gr-c-s-loaded="true"
  style="text-align: center;margin: 20px;">
    <h1>Payment Processing</h1>
   <%-- <table border="1" style="width: 100%;margin-bottom: 30px;">
      <tbody>
        <tr>
          <td><b>Parameter Name</b></td>
          <td><b>Parameter Value</b></td>
        </tr>
      </tbody>
    </table>--%>

      <br />
    <div style="margin-top: 40px;">
      <svg width="94px" height="94px" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 100 100" preserveAspectRatio="xMidYMid"
        class="lds-ellipsis" style="background: none;">
        <!--circle(cx="16",cy="50",r="10")-->
        <circle cx="84" cy="50" r="0" fill="#90ffb5">
          <animate attributeName="r" values="10;0;0;0;0" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="0s"></animate>
          <animate attributeName="cx" values="84;84;84;84;84" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="0s"></animate>
        </circle>
        <circle cx="84" cy="50" r="0.000668526" fill="#081bd3">
          <animate attributeName="r" values="0;10;10;10;0" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="-0.5s"></animate>
          <animate attributeName="cx" values="16;16;50;84;84" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="-0.5s"></animate>
        </circle>
        <circle cx="83.9977" cy="50" r="10" fill="#dae209">
          <animate attributeName="r" values="0;10;10;10;0" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="-0.25s"></animate>
          <animate attributeName="cx" values="16;16;50;84;84" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="-0.25s"></animate>
        </circle>
        <circle cx="49.9977" cy="50" r="10" fill="#ed2b05">
          <animate attributeName="r" values="0;10;10;10;0" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="0s"></animate>
          <animate attributeName="cx" values="16;16;50;84;84" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="0s"></animate>
        </circle>
        <circle cx="16" cy="50" r="9.99933" fill="#90ffb5">
          <animate attributeName="r" values="0;0;10;10;10" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="0s"></animate>
          <animate attributeName="cx" values="16;16;16;50;84" keyTimes="0;0.25;0.5;0.75;1"
            keySplines="0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1;0 0.5 0.5 1" calcMode="spline" dur="1s" repeatCount="indefinite"
            begin="0s"></animate>
        </circle>
      </svg>

      <p>Please do not close the page or application.Give us some time to process.</p>
    </div>
  </body>

</html>
