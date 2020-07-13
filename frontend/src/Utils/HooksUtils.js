import {useState} from "react";

function useForceUpdate(){
    const [, setDummy] = useState(0)
    return () => setDummy(value => ++value);
}

export {useForceUpdate}