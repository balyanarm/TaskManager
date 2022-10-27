import { useRouter } from "next/router";
import MainContainer from "../../components/MainContainer";

export default function({task}){
    const router = useRouter();
   return(
    <>
    <MainContainer>
    <div>Task ID - {router.query.id}</div>
    <div>Name - {task.name}</div>
    </MainContainer>
    </>
    
   )
};

  
export async function getServerSideProps({params}){
   // const response =await fetch('https://localhost:7179/api/Task/getAll')
    const response =await fetch(`https://jsonplaceholder.typicode.com/users/${params.id}`)
    console.log(response);
    const task = await response.json()
       return {
        props: {task},
       }
}