const getData = async () => {

    const apiServer = process.env['services__webapi__https__0'] ?? process.env['services__webapi__http__0'];
    const weatherData: Response = await fetch(`${apiServer}/api/weatherforecast`, { cache: 'no-cache' });

    if (!weatherData.ok) {
        throw new Error('Failed to fetch data.');
    }

    const data = await weatherData.json();

    return data
}

const Page = async () => {
    const data = await getData()

    return <main>{JSON.stringify(data)}</main>
}

export default Page